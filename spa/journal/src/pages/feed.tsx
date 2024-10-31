import React from "react";

import DefaultLayout from "../layouts/default.tsx";

export default function FeedPage() {
  return (
    <DefaultLayout>
      <section className="flex flex-col items-center justify-center gap-4 py-8 md:py-10">
        <div className="inline-block max-w-lg text-center justify-center">
          <h1>Feed</h1>
        </div>
      </section>
    </DefaultLayout>
  );
}
