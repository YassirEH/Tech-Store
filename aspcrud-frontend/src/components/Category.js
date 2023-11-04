import React, { useState, useEffect } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';

function CategoryList() {
  const [categories, setCategories] = useState([]);
  const [selectedCategoryId, setSelectedCategoryId] = useState(null);
  const [productsInCategory, setProductsInCategory] = useState([]);
  const [newCategoryName, setNewCategoryName] = useState("");
  const [editCategoryName, setEditCategoryName] = useState("");

  useEffect(() => {
    loadCategories();
  }, []);

  async function loadCategories() {
    try {
      const result = await axios.get("https://localhost:7156/api/Category/Get All Categories");

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
    setEditCategoryName(""); // Clear edit input
    loadProductsInCategory(categoryId);
  }

  async function addCategory() {
    if (!newCategoryName) {
      alert('Category Name is required.');
      return;
    }

    try {
      const response = await axios.post("https://localhost:7156/api/Category/Create Category", {
        name: newCategoryName
      });

      if (response.status === 201) {
        alert("Category added successfully");
        setNewCategoryName(""); // Clear input
        loadCategories();
      }
    } catch (err) {
      alert(err);
    }
  }

  async function updateCategory() {
    if (!selectedCategoryId || !editCategoryName) {
      alert('Category and Category Name are required for editing.');
      return;
    }

    try {
      await axios.put(`https://localhost:7156/api/Category/Update Category/${selectedCategoryId}`, {
        name: editCategoryName
      });

      alert("Category updated successfully");
      setEditCategoryName(""); // Clear edit input
      loadCategories();
    } catch (err) {
      alert(err);
    }
  }

  async function deleteCategory() {
    if (!selectedCategoryId) {
      alert('Select a category to delete.');
      return;
    }

    try {
      await axios.delete(`https://localhost:7156/api/Category/Delete Category/${selectedCategoryId}`);

      alert("Category deleted successfully");
      setSelectedCategoryId(null);
      setEditCategoryName(""); // Clear edit input
      loadCategories();
    } catch (err) {
      alert(err);
    }
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
                  className={`btn btn-link ${selectedCategoryId === category.id ? 'active' : ''}`}
                  onClick={() => selectCategory(category.id)}
                >
                  {category.name}
                </button>
              </li>
            ))}
          </ul>
          <div className="form-group">
            <input
              type="text"
              className="form-control"
              placeholder="New Category Name"
              value={newCategoryName}
              onChange={(event) => setNewCategoryName(event.target.value)}
            />
            <button className="btn btn-primary mt-2" onClick={addCategory}>Add Category</button>
          </div>
        </div>

        <div className="col">
          <h3>Products in Selected Category</h3>
          {selectedCategoryId === null ? (
            <p>Select a category to view products.</p>
          ) : (
            <div>
              <ul>
                {productsInCategory.map((product) => (
                  <li key={product.id}>
                    {product.name}
                  </li>
                ))}
              </ul>
              <div className="form-group">
                <input
                  type="text"
                  className="form-control"
                  placeholder="Edit Category Name"
                  value={editCategoryName}
                  onChange={(event) => setEditCategoryName(event.target.value)}
                />
                <button className="btn btn-success mt-2" onClick={updateCategory}>Update Category</button>
                <button className="btn btn-danger mt-2" onClick={deleteCategory}>Delete Category</button>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default CategoryList;
