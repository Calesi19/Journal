import {
  createUserWithEmailAndPassword,
  signInWithEmailAndPassword,
  signOut,
} from "firebase/auth";

// Sign up function
export const signUp = (email, password) =>
  createUserWithEmailAndPassword(auth, email, password);

// Sign in function
export const signIn = (email, password) =>
  signInWithEmailAndPassword(auth, email, password);

// Sign out function
export const signOutUser = () => signOut(auth);
