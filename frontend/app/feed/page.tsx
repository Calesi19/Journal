"use client";

import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter } from "@nextui-org/react";
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
  const [selectedPost, setSelectedPost] = useState<PostType | null>(null);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  const openEditModal = (post: PostType) => {
    setSelectedPost(post);
    setIsEditModalOpen(true);
  };

  const closeEditModal = () => {
    setIsEditModalOpen(false);
    setSelectedPost(null);
  };

  const openDeleteModal = (post: PostType) => {
    setSelectedPost(post);
    setIsDeleteModalOpen(true);
  };

  const closeDeleteModal = () => {
    setIsDeleteModalOpen(false);
    setSelectedPost(null);
  };

  useEffect(() => {
    async function fetchPosts() {
      try {
        const response = await axiosInstance.get("/posts");
        const fetchedPosts = response.data.posts;
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

      <div className="w-full">

        <div className="w-full mb-4">
          <NewPost onPostCreated={handleNewPost} />
        </div>
        <div className="w-full h-full overflow-scroll hide-scrollbar">
          {loading ? (
            <>
              <PostSkeleton />
              <PostSkeleton />
            </>
          ) : (
            posts.map((post) => (
              <Post key={post.id} post={post} onEdit={openEditModal} onDelete={openDeleteModal} />
            ))
          )}
          <div className="h-[400px]" />

        </div>

      </div>

      <DeletePostModal isOpen={isDeleteModalOpen} onClose={closeDeleteModal} post={selectedPost} />
      <EditPostModal isOpen={isEditModalOpen} onClose={closeEditModal} post={selectedPost} />
    </section>
  );
}



function NewPost({ onPostCreated }: {
  onPostCreated: (post: PostType) =>
    void
}): React.JSX.Element {
  const [content, setContent] = React.useState("");
  const [loading, setLoading] = React.useState(false);

  const handlePost = async () => {
    if (!content.trim()) {
      alert("Post content cannot be empty.");
      return;
    }

    setLoading(true);

    try {
      const response = await axiosInstance.post("/posts", {
        content,
      });

      const newPostId = response.data.postId;

      const newPost: PostType = {
        id: newPostId,
        content,
        dateCreated: new Date().toISOString(),
      };

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
  post,
  onEdit,
  onDelete,
}: {
  post: PostType;
  onEdit: (post: PostType) => void;
  onDelete: (post: PostType) => void;
}): React.JSX.Element {
  return (
    <div className="mb-10 group">
      <Card>
        <CardHeader className="pb-0 pt-2 flex-col items-start">
          <p className="text-tiny text-default-500 uppercase font-bold">
            {new Date(post.dateCreated).toLocaleDateString()}
          </p>
        </CardHeader>
        <CardBody
          className="overflow-visible py-3"
          style={{ whiteSpace: "pre-wrap" }}
        >
          {post.content}
        </CardBody>
      </Card>
      <div className="text-transparent group-hover:text-white flex flex-row-reverse transition ease-in-out duration-300 ">
        <a
          href="#"
          onClick={(e) => {
            e.preventDefault();
            onEdit(post);
          }}
        >
          edit
        </a>
        <a
          href="#"
          className="hover:text-red-500 mr-4"
          onClick={(e) => {
            e.preventDefault();
            onDelete(post);
          }}
        >
          remove
        </a>

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
          <a onClick={SignOut} className="hover:text-red-500">
            Sign Out
          </a>
        </div>
      </div>
    </section>
  );
}

function SignOut() {
  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
  window.location.href = "/login";
}


type EditPostModalProps = {
  isOpen: boolean;
  onClose: () => void;
  post: PostType | null;
};


export function EditPostModal({ isOpen, onClose, post }: EditPostModalProps) {
  const [content, setContent] = useState(post?.content || "");
  const [date, setDate] = useState(post?.dateCreated || "");

  useEffect(() => {
    if (post) {
      setContent(post.content);
      setDate(new Date(post.dateCreated).toLocaleDateString());
    }
  }, [post, isOpen]);

  const handleSave = async () => {
    if (!content.trim()) {
      alert("Post content cannot be empty.");
      return;
    }

    try {
      // Update the post via API
      await axiosInstance.put(`/posts/${post?.id}`, { content });

      alert("Post updated successfully!");
      onClose();
      // Optionally: Refresh the posts list
    } catch (error) {
      console.error("Error updating post:", error);
      alert("Failed to update post.");
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isDismissable={false}>
      <ModalContent>
        <ModalHeader>
          Edit Post
        </ModalHeader>
        <ModalBody>
          <p className="text-sm text-gray-500">
            {date ? `Created on: ${date}` : ""}
          </p>
          <Textarea
            size="sm"
            type="text"
            value={content}
            onChange={(e) => setContent(e.target.value)}
            isRequired
          />
        </ModalBody>
        <ModalFooter>
          <Button onClick={onClose}>
            Cancel
          </Button>
          <Button color="primary" onClick={handleSave}>
            Save
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}


export function DeletePostModal({ isOpen, onClose, post }: EditPostModalProps) {
  const [content, setContent] = useState(post?.content || "");
  const [date, setDate] = useState(post?.dateCreated || "");

  useEffect(() => {
    if (post) {
      setContent(post.content);
      setDate(new Date(post.dateCreated).toLocaleDateString());
    }
  }, [post, isOpen]);

  const handleSave = async () => {
    if (!content.trim()) {
      alert("Post content cannot be empty.");
      return;
    }

    try {
      // Update the post via API
      await axiosInstance.delete(`/posts/${post?.id}`, { content });

      alert("Post updated successfully!");
      onClose();
      // Optionally: Refresh the posts list
    } catch (error) {
      console.error("Error updating post:", error);
      alert("Failed to update post.");
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalContent>
        <ModalHeader>
          Delete Post
        </ModalHeader>
        <ModalBody>
          <p className="text-sm text-gray-500">
            {date ? `Created on: ${date}` : ""}
          </p>
          <Textarea
            size="sm"
            type="text"
            value={content}
            disabled
          />
        </ModalBody>
        <ModalFooter>
          <Button onClick={onClose}>
            Cancel
          </Button>
          <Button color="danger" onClick={handleSave}>
            Delete Post
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
