import * as functions from "firebase-functions";
import db from "./utils/db";
import moment from "moment-timezone";
import lambdaClient from "./utils/aws/lambdaClient";
import {InvokeCommand} from "@aws-sdk/client-lambda";
import {firestore} from "firebase-admin";

const DEFAULT_TIMEZONE = "America/New_York"; // ET
const GENERATE_CODLE_FUNCTION_NAME = "GenerateCodle";

async function getNewPuzzle() {
  const generateCodleCommand = new InvokeCommand({
    FunctionName: GENERATE_CODLE_FUNCTION_NAME,
  });

  const {Payload: newCodlePayload} = await lambdaClient.send(generateCodleCommand);
  const newCodle = JSON.parse(Buffer.from(newCodlePayload as any).toString("utf-8"));

  return newCodle;
}

export const persistCodle = functions.pubsub
    .schedule("55 23 * * *")
    .timeZone(DEFAULT_TIMEZONE) // ET
    .onRun(async (_context) => {
      const puzzles = db.collection("puzzles");

      const currentTime = moment().tz(DEFAULT_TIMEZONE).toDate();

      const currentTimeStart = moment(currentTime)
        .tz(DEFAULT_TIMEZONE)
        .endOf("day")
        .subtract(5, "minutes");
      const currentTimeEnd = moment(currentTime)
        .tz(DEFAULT_TIMEZONE)
        .endOf("day");

      const isBetweenThreshold = moment(currentTime)
      .tz(DEFAULT_TIMEZONE)
          .isBetween(currentTimeStart, currentTimeEnd, undefined, "[)");
      const dayIncrement = isBetweenThreshold ? 1 : 0;

      const startDate = moment()
          .tz(DEFAULT_TIMEZONE)
          .add(dayIncrement, "day")
          .startOf("day")
          .toDate();

      const endDate = moment()
          .tz(DEFAULT_TIMEZONE)
          .add(dayIncrement, "day")
          .endOf("day")
          .toDate();

      const {size} = await puzzles
          .orderBy("timestamp")
          .startAt(startDate)
          .endBefore(endDate)
          .limit(1)
          .get();

      if (size) return;

      const newCodle = await getNewPuzzle();
      const startOfDay = moment(startDate);
      const startOfDayUnixTS = startOfDay.unix(); // seconds

      await puzzles.add({
        sequence: newCodle.puzzle,
        timestamp: new firestore.Timestamp(startOfDayUnixTS, 0),
      });
    });
