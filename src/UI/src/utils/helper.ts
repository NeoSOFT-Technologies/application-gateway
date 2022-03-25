import moment from "moment";

const helper = (item: any) => {
  const listObj = Object.create(item);
  listObj.Status = listObj.IsActive === true ? "Active" : "In-Active";
  listObj.CreatedDateTxt =
    listObj.CreatedDate !== ""
      ? moment(listObj.CreatedDate).format("DD/MM/YYYY")
      : "";
  return listObj;
};
export default helper;
