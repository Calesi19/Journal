"use client";
import React from "react";
import {
  Tabs,
  Tab,
  Card,
  CardBody,
  Input,
  CardHeader,
  Button,
} from "@nextui-org/react";

export default function LoginPage(): React.JSX.Element {
  return (
    <section className="flex container justify-center items-center w-full h-full overflow-hidden">
      <Login />
    </section>
  );
}

function Login(): React.JSX.Element {
  return (
    <div className="flex w-full max-w-[400px] flex-col">
      <h1 className="text-center py-8 font-black text-8xl">journal</h1>
      <Card>
        <CardBody>
          <Tabs fullWidth aria-label="Dynamic tabs">
            <Tab title="Log In">
              <Input size="sm" placeholder="Email" className="mt-6" />
              <Input size="sm" placeholder="Password" className="mt-4" />
              <Button className="mt-4" color="primary" fullWidth>
                Log In
              </Button>
            </Tab>{" "}
            <Tab title="Create Account">
              <Input size="sm" placeholder="Email" className="mt-6" />
              <Input size="sm" placeholder="Password" className="mt-4" />
              <Input size="sm" placeholder="Confirm Password" className="mt-4" />
              <Button className="mt-4" color="primary" fullWidth>
                Log In
              </Button>
            </Tab>
          </Tabs>
        </CardBody>
          </Card>
          <div>
            <p className="text-center mt-4">
              <a href="#" className="text-default-300">
                Forgot Password?
              </a>
            </p>
          </div>
    </div>
  );
}
