"use client";

import React, { useState, Suspense, useEffect } from "react";
import { Tabs, Tab, Card, CardBody, Input, Button } from "@nextui-org/react";
import axiosInstance from "../../utils/axiosInstance"; // Import Axios instance
import { signUp } from "../../utils/auth"; // Keep signUp function if needed
import { useSearchParams } from "next/navigation";

export default function LoginPage(): React.JSX.Element {
  return (
    <section className="flex container justify-center items-center w-full h-full overflow-hidden">
      <div className="absolute top-1/4 transform -translate-y-1/4">
        <Suspense fallback={<div>Loading...</div>}>
          <Login />
        </Suspense>
      </div>
    </section>
  );
}

function Login(): React.JSX.Element {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [selectedTab, setSelectedTab] = useState<string>("login"); // Default to "Log In"
  const searchParams = useSearchParams(); // Get query parameters

  useEffect(() => {
    const tab = searchParams.get("tab");
    if (tab === "create-account") {
      setSelectedTab("create-account");
    }
  }, [searchParams]);

  useEffect(() => {
    const checkTokens = async () => {
      const accessToken = localStorage.getItem("accessToken");
      const refreshToken = localStorage.getItem("refreshToken");

      if (!refreshToken) {
        // No refreshToken, clear localStorage and return
        localStorage.clear();
        return;
      }

      try {
        if (accessToken) {
          // Validate accessToken
          const accessTokenResponse = await axiosInstance.post("/validate-token", {
            token: accessToken,
          });

          if (accessTokenResponse.data.valid) {
            // Redirect to /feed if accessToken is valid
            window.location.href = "/feed";
            return;
          }
        }

        // If accessToken is invalid, try refreshing it
        const refreshResponse = await axiosInstance.post("/refresh-token", {
          refreshToken,
        });

        if (refreshResponse.data.success) {
          // Store the new accessToken and redirect
          localStorage.setItem("accessToken", refreshResponse.data.accessToken);
          window.location.href = "/feed";
          return;
        } else {
          // Refresh token is invalid, clear storage
          localStorage.clear();
        }
      } catch (error) {
        console.error("Token validation/refresh error:", error);
        localStorage.clear();
      }
    };

    checkTokens();
  }, []);

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
      window.location.href = "/feed";
    } catch (error) {
      console.error("Error logging in:", (error as Error).message);
    }
  };

  const handleCreateAccountSubmit = async (
    event: React.FormEvent<HTMLFormElement>
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
      <h1 className="text-center py-8 font-black text-8xl"><a href="/">journal</a></h1>
      <Card>
        <CardBody>
          <Tabs
            fullWidth
            aria-label="Dynamic tabs"
            selectedKey={selectedTab}
            onSelectionChange={(key) => setSelectedTab(key as string)}
          >
            <Tab title="Log In" key="login">
              <form onSubmit={handleLoginSubmit}>
                <Input
                  size="sm"
                  placeholder="Email"
                  className="mt-6"
                  isRequired
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
                <Input
                  size="sm"
                  type="password"
                  placeholder="Password"
                  className="mt-4"
                  isRequired
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <Button
                  type="submit"
                  className="mt-4"
                  color="primary"
                  fullWidth
                >
                  Log In
                </Button>
              </form>
            </Tab>
            <Tab title="Create Account" key="create-account">
              <form onSubmit={handleCreateAccountSubmit}>
                <Input
                  size="sm"
                  placeholder="Email"
                  className="mt-6"
                  isRequired
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
                <Input
                  size="sm"
                  type="password"
                  placeholder="Password"
                  className="mt-4"
                  isRequired
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <Input
                  size="sm"
                  type="password"
                  placeholder="Confirm Password"
                  className="mt-4"
                  isRequired
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                />
                <Button
                  type="submit"
                  className="mt-4"
                  color="primary"
                  fullWidth
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
          <a href="#" className="text-default-300 hover:text-blue-500">
            Forgot Password?
          </a>
        </p>
      </div>
    </div>
  );
}

