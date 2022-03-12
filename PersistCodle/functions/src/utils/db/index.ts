import * as admin from "firebase-admin";
require("dotenv").config();

const serviceAccount: admin.ServiceAccount = {
  privateKey: process.env.CODLE_FIREBASE_PRIVATE_KEY,
  clientEmail: process.env.CODLE_FIREBASE_CLIENT_EMAIL,
  projectId: process.env.CODLE_FIREBASE_PROJECT_ID,
};

if (!admin.apps.length) {
  try {
    admin.initializeApp({
      credential: admin.credential.cert(serviceAccount),
    });
  } catch (error:any) {
    console.log("Firebase Admin Initialization Error", error?.stack);
  }
}
export default admin.firestore();
