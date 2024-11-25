import Image from "next/image";
import styles from "./page.module.css";
import HeaderPage from "./pageheader"
import Conver from "./convertString"



export default function Home() {

  let num = 1
  return (
    <div>
         <HeaderPage/>
         <Conver/>
    </div>
  );
}
