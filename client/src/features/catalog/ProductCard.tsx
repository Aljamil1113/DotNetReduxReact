
import { LoadingButton } from "@mui/lab";
import { Avatar, Button, Card, CardActions, CardContent, CardHeader, CardMedia, Typography} from "@mui/material";
import { Link } from "react-router-dom";
import { Product } from "../../app/models/products";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { currencyFormat } from "../../app/util/util";
import { addBasketItemAsync, setBasket } from "../basket/basketSlice";

export default function ProductCard({product} : Props) {
    const {status} = useAppSelector(state => state.basket);
    const dispatch = useAppDispatch();

    return (
        <>
           <Card>
            <CardHeader avatar={
                <Avatar sx={{bgcolor: 'secondary.main'}}>
                    {currencyFormat(product.price)}
                </Avatar>
            } 
                title={product.name}  
                titleTypographyProps={{
                    sx: {fontWeight: 'bold'}
                }}
                />
            <CardMedia sx={{ height: 140, backgroundSize: 'contain', bgcolor: 'primary.light' }} 
                image={product.pictureUrl} title={product.name}/>
            <CardContent>
                <Typography gutterBottom color='secondary' variant="h5"> ${( product.price / 100 ).toFixed(2)} </Typography>
                <Typography variant="body2" color="text.secondary">
                    {product.brand} / {product.type}
                </Typography>
            </CardContent>
            <CardActions>
                <LoadingButton loading={status.includes('pendingAddItem' + product.id)} size="small"
                onClick={() => dispatch(addBasketItemAsync({productId: product.id}))}>Add to cart</LoadingButton>
                <Button component={Link} to={`/catalog/${product.id}`} size="small">View</Button>
            </CardActions>
            </Card>
        </>
    )
}

interface Props {
    product: Product;
}