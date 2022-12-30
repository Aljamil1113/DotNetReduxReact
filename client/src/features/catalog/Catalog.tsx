import { Product } from "../../app/models/products"

export default function Catalog({products, addProduct}: Props) {
    return (
        <>
             <h1>Catalog</h1>
             <ul>
             {products.map(product => (
                <li key={product.id}>{product.name} - {product.price}</li>
             ))}
             </ul>
            <button onClick={addProduct}>Add Product</button>     
        </>
    )
}

interface Props {
    products: Product[];
    addProduct: () => void;
}