import * as functions from "firebase-functions";
import axios from "axios";

const DEFAULT_TIMEZONE = "America/New_York"; // ET

interface INextServerResponse {
  revalidated: boolean;
  message?: string;
  error?: boolean;
}

export const refreshCache = functions.pubsub
    .schedule("0 0 * * *")
    .timeZone(DEFAULT_TIMEZONE) // ET
    .retryConfig({retryCount: 3})
    .onRun(async (_context) => {
      const response:INextServerResponse = await axios.get("/revalidate", {
        baseURL: "https://codle.mahitm.com/api",
        params: {
          secret: process.env.CODLE_FORCE_REVALIDATE_TOKEN,
        },
      });
      if (response.revalidated) {
        return Promise.resolve("Revalidated Cache Successfully!");
      } else if (response.error && !response.revalidated) {
        return Promise.reject(new Error("Revalidating Cache Failed."));
      } else {
        return Promise.resolve(response.message);
      }
    });
