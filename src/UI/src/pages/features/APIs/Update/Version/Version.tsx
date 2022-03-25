import React from "react";
import { changeApiUpdateForm } from "../../../../../resources/common";
import { IProps } from "../../../../../types/api";
import Versions from "./Versions/Versions";
import VersionSettings from "./VersionSettings/VersionSettings";

export default function Version(props: IProps) {
  return (
    <div>
      <div>
        <VersionSettings
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            changeApiUpdateForm(e, props)
          }
        />
        <Versions
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            changeApiUpdateForm(e, props)
          }
        />
      </div>
    </div>
  );
}
