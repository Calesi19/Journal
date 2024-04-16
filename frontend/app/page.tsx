"use client";

import {
  Input,
  ScrollShadow,
  Card,
  CardHeader,
  CardBody,
  Textarea,
  Button,
} from "@nextui-org/react";
import { FaSearch } from "react-icons/fa";

// import { TabsFAQ } from "./faq";
// import { BouncyCardsFeatures } from "./contact";

// import AccordionSolutions from "./about";
import React from "react";

export default function Home(): React.JSX.Element {
  return (
    <section className="flex container w-full h-full overflow-hidden pt-16 gap-16 ">
      <div className="w-1/3 hidden md:flex">
        <Menu />
      </div>
      <div className="md:w-2/3 overflow-scroll hide-scrollbar">
        <NewPost />
        <OldPost />
        <OldPost />
        <OldPost />
        <OldPost />
        <OldPost />
        <OldPost />
        <OldPost />
        <OldPost />
      </div>
    </section>
  );
}

function NewPost(): React.JSX.Element {
  const [selectedKeys, setSelectedKeys] = React.useState(new Set(["text"]));

  const selectedValue = React.useMemo(
    () => Array.from(selectedKeys).join(", ").replaceAll("_", " "),
    [selectedKeys]
  );

  return (
    <section className="flex flex-col gap-2 ">
      <Textarea minRows={10} />
      <div className="flex justify-between">
        <div className="flex gap-2"></div>
        <Button>Post</Button>
      </div>
    </section>
  );
}

function OldPost(): React.JSX.Element {
  return (
    <div className="my-10 group">
      <Card className="">
        <CardHeader className="pb-0 pt-2 flex-col items-start">
          <p className="text-tiny text-default-500 uppercase font-bold">
            march 19, 2024
          </p>
        </CardHeader>
        <CardBody className="overflow-visible py-2">
          Lorem Ipsum is simply dummy text of the printing and typesetting
          industry. Lorem Ipsum has been the industry's standard dummy text ever
          since the 1500s, when an unknown printer took a galley of type and
          scrambled it to make a type specimen book. It has survived not only
          five centuries, but also the leap into electronic typesetting,
          remaining essentially unchanged. It was popularised in the 1960s with
          the release of Letraset sheets containing Lorem Ipsum passages, and
          more recently with desktop publishing software like Aldus PageMaker
          including versions of Lorem Ipsum.
        </CardBody>
      </Card>
      <div className=" text-transparent group-hover:text-white flex flex-row-reverse">
        <a href="#">edit</a>
      </div>
    </div>
  );
}

function Menu(): React.JSX.Element {
  return (
    <section className="hidden md:flex flex-col justify-between">
      <div className="h-[200px]">
        <Input
          classNames={{
            mainWrapper: "h-full",

            inputWrapper:
              "h-full font-normal text-default-500 bg-default-400/20 dark:bg-default-500/20",
          }}
          placeholder="Type to search..."
          startContent={<FaSearch size={18} />}
          type="search"
          fullWidth
        />
        <ScrollShadow className=" hide-scrollbar">
          <ul className="pt-16">
            <li className="py-2 hover:text-red-200">
              <a href="#">church</a>
            </li>
            <li className="py-2">stoic</li>
            <li className="py-2">money</li>
            <li className="py-2">life</li>
            <li className="py-2 hover:text-red-200">
              <a href="#">church</a>
            </li>
            <li className="py-2">stoic</li>
            <li className="py-2">money</li>
            <li className="py-2">life</li>
            <li className="py-2 hover:text-red-200">
              <a href="#">church</a>
            </li>
            <li className="py-2">stoic</li>
            <li className="py-2">money</li>
            <li className="py-2">life</li>
            <li className="py-2 hover:text-red-200">
              <a href="#">church</a>
            </li>
            <li className="py-2">stoic</li>
            <li className="py-2">money</li>
            <li className="py-2">life</li>
          </ul>
        </ScrollShadow>
      </div>
      <div>
        <div className="pb-4">
          <a href="/settings">Settings</a>
        </div>
        <div className="pb-16">
          <a href="/login">Sign Out</a>
        </div>
      </div>
    </section>
  );
}
