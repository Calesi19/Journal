import { Button, Input, Modal, ModalContent, ModalHeader, ModalBody, ModalFooter } from "@nextui-org/react";
import { useState } from "react";

type EditPostModalProps = {
  isOpen: boolean;
  onClose: () => void;
  post: PostType | null;
};

export default function EditPostModal({ isOpen, onClose, post }: EditPostModalProps) {
  const [content, setContent] = useState(post?.content || "");

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
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalContent>
        <ModalHeader>Edit Post</ModalHeader>
        <ModalBody>
          <Input
            size="sm"
            type="text"
            value={content}
            onChange={(e) => setContent(e.target.value)}
            placeholder="Post content"
            isRequired
          />
        </ModalBody>
        <ModalFooter>
          <Button auto flat onClick={onClose}>
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

