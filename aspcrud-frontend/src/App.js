import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import ProductList from "./components/Products";
import CategoryList from "./components/Category";
import BuyerList from "./components/Buyer";



function App() {
  return (
    <Router>
      <div className="App">
      <nav className="navbar navbar-expand-lg navbar-light bg-light">
    <ul className="navbar-nav">
      <li className="nav-item">
        <Link to="/" className="nav-link">
          Home
        </Link>
      </li>
      <li className="nav-item">
        <Link to="/products" className="nav-link">
          Products
        </Link>
      </li>
      <li className="nav-item">
        <Link to="/categories" className="nav-link">
          Categories
        </Link>
      </li>
      <li className="nav-item">
        <Link to="/buyers" className="nav-link">
          Users
        </Link>
      </li>
    </ul>
    
  </nav>

        <Routes>
          <Route path="/" element={<h1>Home Page</h1>} />
          <Route path="/products" element={<ProductList />} />
          <Route path="/buyers" element={<BuyerList />} />
          <Route path="/categories" element={<CategoryList />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
