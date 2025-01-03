"use client";

import React, { useState, Suspense, useEffect } from "react";
import { Tabs, Tab, Card, CardBody, Input, Button, CardHeader } from "@nextui-org/react";
import axiosInstance from "../../utils/axiosInstance"; // Import Axios instance
import { useSearchParams } from "next/navigation";


export default function PasswordResetPage(): React.JSX.Element {
    return (
        <section className="flex container justify-center items-center w-full h-full overflow-hidden">
            <div className="absolute top-1/4 transform -translate-y-1/6">
                <Suspense fallback={<div>Loading...</div>}>
                    <EmailBox />
                </Suspense>
            </div>
        </section>
    );
}


function EmailBox() {
    return (
        <div className="w-full max-w-[400px]">
            <Card>
                <CardHeader className="flex flex-col items-start ">
                    <h1 className="font-semibold"> Reset Password</h1>
                    <p>Enter your email address to reset your password.</p>
                </CardHeader>
                <CardBody>
                    <Input
                        size="sm"
                        type="email"
                        placeholder="Email"
                        isRequired
                        className="mb-4" />
                    <Button
                        color="primary"
                        type="submit"
                    >
                        Send Email
                    </Button>
                </CardBody>
            </Card>

        </div>
    )
}
