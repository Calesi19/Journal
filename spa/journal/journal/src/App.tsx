import React from "react";
import { type ReactNode } from "react";
import { Route, Router, Switch } from "@miyauci/react-router";
import { useState } from "react";
import "./App.css";
import LoginPage from "./assets/login/page";

function App() {
  return (
    <Router>
      <Switch>
        <Route pathname="/login">
          <LoginPage />
        </Route>
        <Route pathname="/feed">
          <FeedPage />
        </Route>
        <Route pathname="/settings">
          <SettingsPage />
        </Route>
      </Switch>
    </Router>
  );
}

export default App;
