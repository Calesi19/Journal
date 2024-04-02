/* eslint-disable @typescript-eslint/no-base-to-string */

import React from "react";
import {
  Navbar as NextUINavbar,
  NavbarContent,
  NavbarBrand,
  NavbarItem,
} from "@nextui-org/react";

import NextLink from "next/link";

import { ThemeSwitch } from "../components/theme-switch";


function Logo(): React.JSX.Element {
  return (
    <div id="logo" className="h-[32px] w-[128px] relative font-bold">
      <div
        id="prll"
        className={`h-[32px] w-[62px] logo-skew rounded-lg absolute left-[2px] bg-black dark:bg-white`}
      ></div>
      <span
        id="first-name"
        className={`absolute top-[3px] left-[5px] text-[20.5px] text-white dark:text-black `}
      >
        carlos
      </span>
      <span
        id="last-name absolute"
        className={`absolute top-[3px] left-[65px] text-[20.5px] dark:text-white`}
      >
        lespin
      </span>
    </div>
  );
}

export const Navbar = (): React.JSX.Element => {
  return (
    <NextUINavbar
      shouldHideOnScroll
      position="sticky"
      maxWidth="full"
      className="navfix"
    >
      <NavbarContent className="basis-full" justify="start">
        <NavbarBrand as="li" className="max-w-fit">
          <NextLink className="flex justify-start items-center gap-1" href="/">
            Journal
          </NextLink>
        </NavbarBrand>
      </NavbarContent>

      <NavbarContent className="flex basis-1/5 sm:basis-full" justify="end">
        <NavbarItem className="flex gap-3">
        
          <ThemeSwitch />
        </NavbarItem>
      </NavbarContent>
    </NextUINavbar>
  );
};
