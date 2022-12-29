import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';

function App() {

  const [products, setProducts] = useState([
    {name: 'product1', price: 100.00}
  ])

  useEffect(() => {
    fetch('https://localhost:7280/api/products')
    .then(response => response.json())
    .then(data => setProducts(data));
  }, []);

  return (
    <div>
        <h1>Re-Store</h1>

        {products.map((item, index) => (
          <li key={index}>{item.name} - {item.price}</li>
        ))}
    </div>
  );
}

export default App;
