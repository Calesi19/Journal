"use client";

import {
  Input,
  ScrollShadow,
  Card,
  CardHeader,
  CardBody,
  Textarea,
  Button,
  Skeleton,
} from "@nextui-org/react";
import { FaSearch } from "react-icons/fa";
import React, { useEffect, useState } from "react";
import axiosInstance from "../../utils/axiosInstance";

type PostType = {
  id: number;
  content: string;
  dateCreated: string;
};


export default function FeedPage(): React.JSX.Element {
  const [posts, setPosts] = useState<PostType[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchPosts() {
      try {
        const response = await axiosInstance.get("/accounts/posts");
        const fetchedPosts = response.data.response.posts;
        setPosts(fetchedPosts);
      } catch (error) {
        console.error("Error fetching posts:", error);
      } finally {
        setLoading(false);
      }
    }

    fetchPosts();
  }, []);

  // Function to add new post dynamically
  const handleNewPost = (newPost: PostType) => {
    setPosts((prevPosts) => [newPost, ...prevPosts]);
  };

  return (
    <section className="flex container w-full h-full overflow-hidden pt-16 gap-16">
      <div className="w-1/3 hidden md:flex">
        <Menu />
      </div>
      <div className="md:w-2/3 w-full overflow-scroll hide-scrollbar">
        <NewPost onPostCreated={handleNewPost} />
        {loading ? (
          <>
            <PostSkeleton />
            <PostSkeleton />
            <PostSkeleton />
          </>
        ) : (
          posts.map((post: PostType) => (
            <Post
              key={post.id}
              content={post.content}
              date={new Date(post.dateCreated).toLocaleDateString()}
            />
          ))
        )}
      </div>
    </section>
  );
}



function NewPost({ onPostCreated }: { onPostCreated: (post: PostType) => void }): React.JSX.Element {
  const [content, setContent] = React.useState("");
  const [loading, setLoading] = React.useState(false);

  const handlePost = async () => {
    if (!content.trim()) {
      alert("Post content cannot be empty.");
      return;
    }

    setLoading(true);

    try {
      const response = await axiosInstance.post("/accounts/posts", {
        request: { content },
      });

      const newPost: PostType = response.data.response.post;

      // Add the new post to the list
      onPostCreated(newPost);

      // Clear the input field after successful post
      setContent("");
      alert("Post created successfully!");
    } catch (error) {
      console.error("Error creating post:", error);
      alert("Failed to create post.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <section className="flex flex-col gap-2">
      <Textarea
        minRows={2}
        value={content}
        onChange={(e) => setContent(e.target.value)}
        placeholder="What's on your mind?"
      />
      <div className="flex justify-between">
        <div className="flex gap-2"></div>
        <Button onClick={handlePost} isLoading={loading}>
          Post
        </Button>
      </div>
    </section>
  );
}


function Post({
  content,
  date,
}: {
  content: string;
  date: string;
}): React.JSX.Element {
  return (
    <div className="my-10 group ">
      <Card>
        <CardHeader className="pb-0 pt-2 flex-col items-start">
          <p className="text-tiny text-default-500 uppercase font-bold">
            {date}
          </p>
        </CardHeader>
        <CardBody
          className="overflow-visible py-3"
          style={{ whiteSpace: "pre-wrap" }}
        >
          {content}
        </CardBody>
      </Card>
      <div className="text-transparent group-hover:text-white flex flex-row-reverse transition ease-in-out duration-300 ">
        <a href="#">edit</a>
      </div>
    </div>
  );
}

function PostSkeleton(): React.JSX.Element {
  return <Skeleton className="my-10 h-[200px] w-full rounded-xl" />;
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
          <a href="/login" className="hover:text-red-500">
            Sign Out
          </a>
        </div>
      </div>
    </section>
  );
}
