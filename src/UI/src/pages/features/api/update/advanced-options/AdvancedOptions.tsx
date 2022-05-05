import React from "react";
import BlacklistedIPs from "./blacklisted-ips/BlacklistedIPs";
import WhitelistedIPs from "./whitelisted-ips/WhitelistedIPs";

export default function AdvancedOptions() {
  return (
    <div>
      <WhitelistedIPs />
      <BlacklistedIPs />
    </div>
  );
}
