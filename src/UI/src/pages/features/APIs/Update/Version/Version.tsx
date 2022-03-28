import React from "react";
import { changeApiUpdateForm } from "../../../../../resources/common";
import { IProps } from "../../../../../types/api";
import Versions from "./Versions/Versions";
import VersionSettings from "./VersionSettings/VersionSettings";

export default function Version(props: IProps) {
  const data = props.updateApiData;
  console.log(data);
  return (
    <div>
      <div>
        <VersionSettings
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            changeApiUpdateForm(e, props)
          }
          updateApiData={data}
        />
        <Versions
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            changeApiUpdateForm(e, props)
          }
          updateApiData={data}
        />
      </div>
    </div>
  );
}
