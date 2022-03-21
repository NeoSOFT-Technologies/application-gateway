import React from "react";
import Versions from "./Versions/Versions";
import VersionSettings from "./VersionSettings/VersionSettings";

export default function Version() {
  return (
    <div>
      Version
      <div>
        <VersionSettings />
        <Versions />
      </div>
    </div>
  );
}
