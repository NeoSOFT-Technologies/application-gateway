import React from "react";
import Versions from "./versions/Versions";
import VersionSettings from "./version-settings/VersionSettings";

export default function Version() {
  return (
    <div>
      <div>
        <VersionSettings />
        <Versions />
      </div>
    </div>
  );
}
