import { Route, Routes } from "react-router-dom";
import React from "react";

import SettingsPage from "./pages/settings.tsx";
import LoginPage from "./pages/login.tsx";
import FeedPage from "./pages/feed.tsx";

function App() {
  return (
    <Routes>
      <Route element={<LoginPage />} path="/" />
      <Route element={<FeedPage />} path="/feed" />
      <Route element={<SettingsPage />} path="/settings" />
    </Routes>
  );
}

export default App;
