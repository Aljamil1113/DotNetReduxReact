import { Avatar, List, ListItem } from "@mui/material";
import Button from "@mui/material/Button";
import ListItemAvatar from "@mui/material/ListItemAvatar";
import ListItemText from "@mui/material/ListItemText";
import { Product } from "../../app/models/products"

export default function Catalog({products, addProduct}: Props) {
    return (
        <>
             <h1>Catalog</h1>
             <List>
             {products.map(product => (
                <ListItem key={product.id}>
                    <ListItemAvatar>
                        <Avatar src={product.pictureUrl} />
                    </ListItemAvatar>
                    <ListItemText>
                        {product.name} - {product.price}
                    </ListItemText>
                </ListItem>
             ))}
             </List>
            <Button variant='contained' onClick={addProduct}>Add Product</Button>     
        </>
    )
}

interface Props {
    products: Product[];
    addProduct: () => void;
}