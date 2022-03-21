import React from "react";
import ListenPath from "./ListenPath/ListenPath";
import RateLimit from "./RateLimit/RateLimit";
import TargetUrl from "./TargetUrl/TargetUrl";

export default function Setting() {
  return (
    <div>
      Setting
      <h1>
        <ListenPath />
        <TargetUrl />
        <RateLimit />
      </h1>
    </div>
  );
}
