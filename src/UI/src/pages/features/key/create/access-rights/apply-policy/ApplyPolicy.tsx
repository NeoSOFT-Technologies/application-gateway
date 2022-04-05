import React from "react";
import Policies from "./policies/Policies";
import PolicyList from "./policy-list/PolicyList";

export default function ApplyPolicy() {
  return (
    <div>
      <h4>Apply policy</h4>
      <PolicyList />
      <Policies />
    </div>
  );
}
