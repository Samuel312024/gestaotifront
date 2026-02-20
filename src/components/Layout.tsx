import Sidebar from "./Sidebar";
import Topbar from "./Topbar";
import { Outlet } from "react-router-dom";

export default function Layout() {
  return (
    <div style={styles.container}>
      <Sidebar />

      <div style={styles.main}>
        <Topbar />
        <div style={styles.content}>
          <Outlet />
        </div>
      </div>
    </div>
  );
}

const styles = {
  container: {
    display: "flex",
    height: "100vh",
    backgroundColor: "#111"
  },
  main: {
    flex: 1,
    display: "flex",
    flexDirection: "column" as const
  },
  content: {
    padding: "30px",
    overflowY: "auto" as const
  }
};
