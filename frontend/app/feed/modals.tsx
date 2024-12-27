import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter } from "@nextui-org/react";
import {
  Textarea,
  Button,
} from "@nextui-org/react";
import axiosInstance from "../../utils/axiosInstance";
import { useEffect, useState } from "react";

type ModalProps = {
  isOpen: boolean;
  onClose: () => void;
  post: PostType | null;
};

type PostType = {
  id: number;
  content: string;
  dateCreated: string;
};

export function EditPostModal({ isOpen, onClose, post }: ModalProps) {
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


export function DeletePostModal({ isOpen, onClose, post }: ModalProps) {
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
      await axiosInstance.delete(`/posts/${post?.id}`);

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
