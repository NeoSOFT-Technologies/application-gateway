import React, { useState } from "react";
import { IProps } from "../types/api";

export function setForm() {
  const [form, setForma] = useState<any>({});
  return [form, setForma];
}
export function err() {
  const [errors, setErrors] = useState<any>({});
  return [errors, setErrors];
}

export function changeApiUpdateForm(
  e: React.ChangeEvent<HTMLInputElement>,
  props: IProps
) {
  props.onChange(e);
}
