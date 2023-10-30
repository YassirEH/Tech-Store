import React, { useState, useEffect } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';

function CategoryList() {
  const [categories, setCategories] = useState([]);
  const [selectedCategoryId, setSelectedCategoryId] = useState(null);
  const [productsInCategory, setProductsInCategory] = useState([]);

  useEffect(() => {
    loadCategories();
  }, []);

  async function loadCategories() {
    try {
      const result = await axios.get("https://localhost:7156/api/Category");

      if (Array.isArray(result.data.data)) {
        setCategories(result.data.data);
      } else {
        console.error("API response does not contain an array:", result.data);
      }
    } catch (err) {
      alert(err);
    }
  }

  async function loadProductsInCategory(categoryId) {
    try {
      const result = await axios.get(`https://localhost:7156/api/ProductCategory/Product/${categoryId}`);

      if (Array.isArray(result.data.data)) {
        setProductsInCategory(result.data.data);
      } else {
        console.error("API response does not contain an array:", result.data);
      }
    } catch (err) {
      alert(err);
    }
  }

  function selectCategory(categoryId) {
    setSelectedCategoryId(categoryId);
    loadProductsInCategory(categoryId);
  }

  return (
    <div className="container mt-4">
      <h1 className="text-center">Categories</h1>

      <div className="row">
        <div className="col">
          <h3>Category List</h3>
          <ul>
            {categories.map((category) => (
              <li key={category.id}>
                <button
                  className="btn btn-link"
                  onClick={() => selectCategory(category.id)}
                >
                  {category.name}
                </button>
              </li>
            ))}
          </ul>
        </div>

        <div className="col">
          <h3>Products in Selected Category</h3>
          {selectedCategoryId === null ? (
            <p>Select a category to view products.</p>
          ) : (
            <ul>
              {productsInCategory.map((product) => (
                <li key={product.id}>
                  {product.name}
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </div>
  );
}

export default CategoryList;
