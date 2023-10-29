import axios from "axios";
import { useEffect, useState } from "react";

function BuyersList() {
  const [Id, setId] = useState("");
  const [FName, setFName] = useState("");
  const [LName, setLName] = useState("");
  const [Email, setEmail] = useState("");
  const [Buyers, setBuyers] = useState(null);

  useEffect(() => {
    (async () => await Load())();
  }, []);

  async function Load() {
    try {
      const result = await axios.get("https://localhost:7156/api/Buyer/Get All Buyers");

      if (Array.isArray(result.data.data)) {
        setBuyers(result.data.data);
        console.log(result.data.data);
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
    setFName(Buyer.fName)
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


  return (
    <div>
      <h1>Buyers List</h1>
      <div className="container mt-4">
        <form>
          <div className="form-group">
            <label>First Name</label>
            <input
              type="text"
              className="form-control"
              id="FName"
              value={FName}
              onChange={(event) => {
                setFName(event.target.value);
              }}
            /><br></br>
            <label>Last Name</label>
            <input
              type="text"
              className="form-control"
              id="LName"
              value={LName}
              onChange={(event) => {
                setLName(event.target.value);
              }}
            />
          </div>
          <div className="form-group">
            <label>Email</label>
            <input
              type="text"
              className="form-control"
              id="Email"
              value={Email}
              onChange={(event) => {
                setEmail(event.target.value);
              }}
            />
          </div>
          <div>
          <button className="btn btn-primary mt-4" onClick={save}>
            Register
          </button>
          <button className="btn btn-warning mt-4" onClick={UpdateBuyer}>
            Update
          </button>
        </div>
        </form>
      </div>
      <br></br>

      <table className="table table-dark" align="center">
        <thead>
          <tr>
          <th scope="col">Buyer Id</th>
            <th scope="col">First Name</th>
            <th scope="col">Last Name</th>
            <th scope="col">Email</th>
          </tr>
        </thead>
        {Buyers === null ? (
          <tbody>
            <tr>
              <td>Loading...</td>
            </tr>
          </tbody>
        ) : (
          Buyers.map(function fn(Buyer, index) {
            return (
              <tbody key={index}>
                <tr>
                  <td>{Buyer.id}</td>
                  <td>{Buyer.fName}</td>
                  <td>{Buyer.lName}</td>
                  <td>{Buyer.email}</td>
                  <td>
                    <button type="button" className="btn btn-warning" onClick={() => {
                        editBuyer(Buyer);
                      }}>
                      Edit
                    </button>
                    <button
                      type="button"
                      className="btn btn-danger"
                      onClick={() => {
                        DeleteBuyer(Buyer.id);
                      }}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              </tbody>
            );
          })
        )}
      </table>
    </div>
  );
}

export default BuyersList;
