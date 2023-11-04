import React, { useState, useEffect } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';

function BuyerList() {
  const [Id, setId] = useState("");
  const [FName, setFName] = useState("");
  const [LName, setLName] = useState("");
  const [Email, setEmail] = useState("");
  const [Buyers, setBuyers] = useState(null);
  const [filterName, setFilterName] = useState("");
  const [filterLastName, setFilterLastName] = useState("");
  const [filteredBuyers, setFilteredBuyers] = useState(null);

  useEffect(() => {
    (async () => await Load())();
  }, []);

  async function Load() {
    try {
      const result = await axios.get("https://localhost:7156/api/Buyer/Get All Buyers");

      if (Array.isArray(result.data.data)) {
        setBuyers(result.data.data);
        setFilteredBuyers(result.data.data);
      } else {
        console.error("API response does not contain an array:", result.data);
      }
    } catch (err) {
      alert(err);
    }
  }

  async function save(event) {
    event.preventDefault();
    try {
      await axios.post("https://localhost:7156/api/Buyer/Create Buyer", {
        fName: FName,
        lName: LName,
        email: Email,
      });
      alert("Buyer Registration Successful");
      setFName("");
      setLName("");
      setEmail("");
      Load();
    } catch (err) {
      alert(err);
    }
  }

  async function editBuyer(Buyer) {
    setFName(Buyer.fName);
    setLName(Buyer.lName);
    setEmail(Buyer.email);
    setId(Buyer.id);
  }

  async function DeleteBuyer(id) {
    if (!id) {
      console.error('Id is not set. Cannot delete the buyer.');
      return;
    }

    try {
      await axios.delete(`https://localhost:7156/api/Buyer/Delete Buyer/${id}`);
      alert("Buyer deleted Successfully");
      setId("");
      setFName("");
      setLName("");
      setEmail("");
      Load();
    } catch (err) {
      alert(err);
    }
  }

  async function UpdateBuyer() {
    if (!Id) {
      console.error('Id is not set. Cannot update the buyer.');
      return;
    }

    try {
      await axios.put(`https://localhost:7156/api/Buyer/Update Buyer?buyerId=${Id}`, {
        fName: FName,
        lName: LName,
        email: Email,
      });
      alert("Buyer updated successfully");
      setId("");
      setFName("");
      setLName("");
      setEmail("");
      Load();
    } catch (err) {
      alert(err);
    }
  }

  function filterBuyersByName() {
    setFilteredBuyers(Buyers);
    const filterValue = filterName.toLowerCase();
    const filteredBuyers = Buyers.filter((buyer) => {
      const buyerFirstName = buyer.fName.toLowerCase();
      return buyerFirstName.includes(filterValue);
    });
    setFilteredBuyers(filteredBuyers);
  }

  function filterBuyersByLastName() {
    setFilteredBuyers(Buyers);
    const filterValue = filterLastName.toLowerCase();
    const filteredBuyers = Buyers.filter((buyer) => {
      const buyerLastName = buyer.lName.toLowerCase();
      return buyerLastName.includes(filterValue);
    });
    setFilteredBuyers(filteredBuyers);
  }

  return (
    <div className="container mt-4">
      <h1 className="text-center">Buyers List</h1>

      <form className="mb-4">
        <div className="form-row">
          <div className="col">
            <input
              type="text"
              className="form-control"
              placeholder="First Name"
              value={FName}
              onChange={(event) => {
                setFName(event.target.value);
              }}
            />
          </div>
          <div className="col">
            <input
              type="text"
              className="form-control"
              placeholder="Last Name"
              value={LName}
              onChange={(event) => {
                setLName(event.target.value);
              }}
            />
          </div>
          <div className="col">
            <input
              type="text"
              className="form-control"
              placeholder="Email"
              value={Email}
              onChange={(event) => {
                setEmail(event.target.value);
              }}
            />
          </div>
          <div className="col">
            <button className="btn btn-primary" onClick={save}>
              Register
            </button>
            <button className="btn btn-warning" onClick={UpdateBuyer}>
              Update
            </button>
          </div>
        </div>
      </form>

      <div className="mb-4">
        <div className="input-group">
          <input
            type="text"
            className="form-control"
            placeholder="Enter First Name"
            value={filterName}
            onChange={(event) => {
              setFilterName(event.target.value);
            }}
          />
          <div className="input-group-append">
            <button className="btn btn-primary" onClick={filterBuyersByName}>
              Search by First Name
            </button>
          </div>
        </div>
      </div>

      <div className="mb-4">
        <div className="input-group">
          <input
            type="text"
            className="form-control"
            placeholder="Enter Last Name"
            value={filterLastName}
            onChange={(event) => {
              setFilterLastName(event.target.value);
            }}
          />
          <div className="input-group-append">
            <button className="btn btn-primary" onClick={filterBuyersByLastName}>
              Search by Last Name
            </button>
          </div>
        </div>
      </div>

      <table className="table table-striped">
        <thead className="thead-dark">
          <tr>
            <th scope="col">Buyer Id</th>
            <th scope="col">First Name</th>
            <th scope="col">Last Name</th>
            <th scope="col">Email</th>
            <th scope="col">Actions</th>
          </tr>
        </thead>
        <tbody>
          {filteredBuyers === null ? (
            <tr>
              <td colSpan="5" className="text-center">Loading...</td>
            </tr>
          ) : (
            filteredBuyers.map(function fn(Buyer, index) {
              return (
                <tr key={index}>
                  <td>{Buyer.id}</td>
                  <td>{Buyer.fName}</td>
                  <td>{Buyer.lName}</td>
                  <td>{Buyer.email}</td>
                  <td>
                    <button
                      type="button"
                      className="btn btn-warning btn-sm"
                      onClick={() => {
                        editBuyer(Buyer);
                      }}
                    >
                      Edit
                    </button>
                    <button
                      type="button"
                      className="btn btn-danger btn-sm"
                      onClick={() => {
                        DeleteBuyer(Buyer.id);
                      }}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              );
            })
          )}
        </tbody>
      </table>
    </div>
  );
}

export default BuyerList;

