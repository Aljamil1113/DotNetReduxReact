import { Button, Divider, Paper, Typography } from "@mui/material";
import { Container } from "@mui/system";

export default function NotFound() {
    return (
        <Container component={Paper} sx={{height: 400}}>
            <Typography gutterBottom variant='h3'>Oops - we could not find what you are looking for</Typography>
            <Divider />
            <Button fullWidth href="/catalog">Go back to shop</Button>
        </Container>
    )
}