"use client";

import React, { useState } from "react";
import { Button } from "@nextui-org/button";
import { Input } from "@nextui-org/input";
import { Card, CardBody } from "@nextui-org/card";

import axiosInstance from "../../utils/axiosInstance"; // Import Axios instance
import { signUp } from "../../utils/auth"; // Keep signUp function if needed

export default function LoginPage(): React.JSX.Element {
  return (
    <section className="flex container justify-center items-center w-full h-full overflow-hidden">
      <Login />
    </section>
  );
}

function Login(): React.JSX.Element {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const handleLoginSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      const response = await axiosInstance.post("/login", {
        request: {
          email: email,
          password: password,
        },
      });

      const { accessToken, refreshToken } = response.data.response;

      console.log("Access Token:", accessToken);
      console.log("Refresh Token:", refreshToken);

      // Store tokens in local storage or context, then redirect if needed
      localStorage.setItem("accessToken", accessToken);
      localStorage.setItem("refreshToken", refreshToken);

      // Redirect to dashboard or home page
    } catch (error) {
      console.error("Error logging in:", (error as Error).message);
    }
  };

  const handleCreateAccountSubmit = async (
    event: React.FormEvent<HTMLFormElement>,
  ) => {
    event.preventDefault();
    if (password !== confirmPassword) {
      alert("Passwords do not match");

      return;
    }
    try {
      await signUp(email, password);
      // Redirect to dashboard or home page
    } catch (error) {
      console.error("Error creating account:", (error as Error).message);
    }
  };

  return (
    <div className="flex w-full max-w-[400px] flex-col">
      <h1 className="text-center py-8 font-black text-8xl">journal</h1>
      <Card>
        <CardBody>
          <Tabs fullWidth aria-label="Dynamic tabs">
            <Tab title="Log In">
              <form onSubmit={handleLoginSubmit}>
                <Input
                  isRequired
                  className="mt-6"
                  placeholder="Email"
                  size="sm"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
                <Input
                  isRequired
                  className="mt-4"
                  placeholder="Password"
                  size="sm"
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <Button
                  fullWidth
                  className="mt-4"
                  color="primary"
                  type="submit"
                >
                  Log In
                </Button>
              </form>
            </Tab>
            <Tab title="Create Account">
              <form onSubmit={handleCreateAccountSubmit}>
                <Input
                  isRequired
                  className="mt-6"
                  placeholder="Email"
                  size="sm"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
                <Input
                  isRequired
                  className="mt-4"
                  placeholder="Password"
                  size="sm"
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <Input
                  isRequired
                  className="mt-4"
                  placeholder="Confirm Password"
                  size="sm"
                  type="password"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                />
                <Button
                  fullWidth
                  className="mt-4"
                  color="primary"
                  type="submit"
                >
                  Create Account
                </Button>
              </form>
            </Tab>
          </Tabs>
        </CardBody>
      </Card>
      <div>
        <p className="text-center mt-4">
          <a className="text-default-300 hover:text-blue-500" href="#">
            Forgot Password?
          </a>
        </p>
      </div>
    </div>
  );
}
