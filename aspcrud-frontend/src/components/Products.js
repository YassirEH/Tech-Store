import React, { useState, useEffect } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';

function ProductList() {
  const [products, setProducts] = useState([]);
  const [selectedProduct, setSelectedProduct] = useState(null);
  const [filterName, setFilterName] = useState("");
  const [filteredProducts, setFilteredProducts] = useState(null);
  const [productName, setProductName] = useState("");
  const [productdescription, setProductdescription] = useState("");
  const [productCategory, setProductCategory] = useState(""); 
  useEffect(() => {
    (async () => await loadProducts())();
  }, []);

  async function loadProducts() {
    try {
      const result = await axios.get("https://localhost:7156/api/Product/Get All");

      if (Array.isArray(result.data.data)) {
        setProducts(result.data.data);
        setFilteredProducts(result.data.data);
      } else {
        console.error("API response does not contain an array:", result.data);
      }
    } catch (err) {
      alert(err);
    }
  }

  async function save(event) {
    event.preventDefault();
    if (!productName) {
      alert('Product Name is required.');
      return;
    }

    // Prepare the product object to be sent to the server
    const newProduct = {
      name: productName,
      description: productdescription,
      category: productCategory, // Assign the selected category
    };

    try {
      const response = await axios.post("https://localhost:7156/api/Product/Create Product/{categoryId}", newProduct);
      if (response.status === 201) {
        alert("Product Registration Successful");
        setProductName("");
        setProductdescription("");
        setProductCategory(""); // Reset category
        loadProducts();
      }
    } catch (err) {
      alert(err);
    }
  }

  async function editProduct(product) {
    setSelectedProduct(product);
    setProductName(product.name);
    setProductdescription(product.description);
    setProductCategory(product.category); // Set the selected category
  }

  async function deleteProduct(productId) {
    if (!productId) {
      console.error('Product ID is not set. Cannot delete the product.');
      return;
    }

    try {
      await axios.delete(`https://localhost:7156/api/Product/DeleteProduct/${productId}`);
      alert("Product deleted Successfully");
      setSelectedProduct(null);
      setProductName("");
      setProductdescription("");
      setProductCategory(""); // Reset category
      loadProducts();
    } catch (err) {
      alert(err);
    }
  }

  async function updateProduct() {
    if (!selectedProduct) {
      console.error('Selected product is not set. Cannot update the product.');
      return;
    }

    // Prepare the updated product object to be sent to the server
    const updatedProduct = {
      id: selectedProduct.id,
      name: productName,
      description: productdescription,
      category: productCategory,
    };

    try {
      await axios.put(`https://localhost:7156/api/Product/UpdateProduct/${selectedProduct.id}`, updatedProduct);
      alert("Product updated successfully");
      setSelectedProduct(null);
      setProductName("");
      setProductdescription("");
      setProductCategory(""); 
      loadProducts();
    } catch (err) {
      alert(err);
    }
  }

  function filterProductsByName() {
    setFilteredProducts(products);
    const filterValue = filterName.toLowerCase();
    const filteredProducts = products.filter((product) => {
      const productName = product.name.toLowerCase();
      return productName.includes(filterValue);
    });
    setFilteredProducts(filteredProducts);
  }

  return (
    <div className="container mt-4">
      <h1 className="text-center">Products List</h1>

      <form className="mb-4" onSubmit={save}>
        <div className="form-row">
          <div className="col">
            <input
              type="text"
              className="form-control"
              placeholder="Product Name"
              value={productName}
              onChange={(event) => {
                setProductName(event.target.value);
              }}
            />
          </div>
          <div className="col">
            <input
              type="text"
              className="form-control"
              placeholder="Product Details"
              value={productdescription}
              onChange={(event) => {
                setProductdescription(event.target.value);
              }}
            />
          </div>
          <div className="col">
            {/* Add an input or select element for product category */}
          </div>
          <div className="col">
            <button type="submit" className="btn btn-primary">Register</button>
          </div>
        </div>
      </form>

      <div className="mb-4">
        <div className="input-group">
          <input
            type="text"
            className="form-control"
            placeholder="Enter Product Name"
            value={filterName}
            onChange={(event) => {
              setFilterName(event.target.value);
            }}
          />
          <div className="input-group-append">
            <button className="btn btn-primary" onClick={filterProductsByName}>
              Search by Name
            </button>
          </div>
        </div>
      </div>

      <table className="table table-striped">
        <thead className="thead-dark">
          <tr>
            <th scope="col">Product ID</th>
            <th scope="col">Product Name</th>
            <th scope="col">Product Description</th>
            <th scope="col">Category</th>
            <th scope="col">Actions</th>
          </tr>
        </thead>
        <tbody>
          {filteredProducts === null ? (
            <tr>
              <td colSpan="5" className="text-center">Loading...</td>
            </tr>
          ) : (
            filteredProducts.map((product, index) => (
              <tr key={index}>
                <td>{product.id}</td>
                <td>{product.name}</td>
                <td>{product.description}</td>
                <td>{product.category}</td>
                <td>
                  <button
                    type="button"
                    className="btn btn-warning btn-sm"
                    onClick={() => {
                      editProduct(product);
                    }}
                  >
                    Edit
                  </button>
                  <button
                    type="button"
                    className="btn btn-danger btn-sm"
                    onClick={() => {
                      deleteProduct(product.id);
                    }}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}

export default ProductList;
