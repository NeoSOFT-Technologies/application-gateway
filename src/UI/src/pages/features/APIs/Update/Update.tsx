import React from "react";
import Setting from "./Setting/Setting";
import Version from "./Version/Version";

export default function Update() {
  return (
    <div>
      Update
      <ul>
        <li>
          <Setting />
        </li>
        <li>
          <Version />
        </li>
      </ul>
    </div>
  );
}
