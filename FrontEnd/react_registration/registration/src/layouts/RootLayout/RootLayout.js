import { Outlet } from "react-router-dom";
import Header from "../../components/Header/Header";
import "./RootLayout.css";
import Sidebar from "../../components/Sidebar/Sidebar";
import { useState } from "react";
export default function RootLayout() {
  const [menuOpen, setMenuOpen] = useState(false);
  return (
    <div className="HomeContainer">
      <Header menuOpen={menuOpen} setMenuOpen={setMenuOpen}></Header>
      <Sidebar menuOpen={menuOpen} setMenuOpen={setMenuOpen}></Sidebar>
      <main>
        <Outlet></Outlet>
      </main>
    </div>
  );
}
