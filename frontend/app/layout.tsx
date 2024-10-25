import "../styles/globals.css";
import type { Metadata } from "next";
import { Providers } from "./providers";
import clsx from "clsx";

import { Analytics } from "@vercel/analytics/react";

export const metadata: Metadata = {
  title: {
    default: "Journal",
    template: `%s - ${"Journal"}`,
  },
  description: "Like Twitter, but private.",
  icons: {
    icon: "/favicon.ico",
    shortcut: "/favicon-16x16.png",
    apple: "/apple-touch-icon.png",
  },
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}): React.JSX.Element {
  return (
    <html lang="en" suppressHydrationWarning className="h-full">
      <head />
      <body
        className={clsx("h-screen w-full bg-background font-sans antialiased")}
      >
        <Analytics />
        <Providers themeProps={{ attribute: "class", defaultTheme: "dark" }}>
          <div className="relative flex flex-col">
            <main className="h-screen">{children}</main>
          </div>
        </Providers>
      </body>
    </html>
  );
}
