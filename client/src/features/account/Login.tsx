import { LoadingButton } from "@mui/lab";
import { Avatar, Box, Container, Grid, Link, Paper, TextField, Typography } from "@mui/material";
import { FieldValues, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import agent from "../../app/api/agent";
import { useAppDispatch } from "../../app/store/configureStore";
import { signInUser } from "./accountSlice";

export default function Login() {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

   const {register, handleSubmit, formState: {isSubmitting, errors, isValid}} = useForm({
    mode: 'all'
   });

   async function submitForm(data: FieldValues) {
        await dispatch(signInUser(data));
        navigate('/catalog'); 
   }

    return (
        <Container component={Paper} maxWidth="sm" sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', p: 4 }}>
        <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
            {/* <LockOutlinedIcon /> */}
        </Avatar>
        <Typography component="h1" variant="h5">
            Sign in
        </Typography>
        <Box component="form" onSubmit={handleSubmit(submitForm)}  noValidate sx={{ mt: 1 }}>
            <TextField
                margin="normal"
                fullWidth
                label="Username"
                autoFocus
                {...register('username', {required: 'Username is required!'})}
                error={!!errors.username}
                helperText={errors?.username?.message?.toString()}
            />
            <TextField
                margin="normal"
                fullWidth
                label="Password"
                type="password"
                {...register('password', {required: 'Password is required'})}
                error={!!errors.password}
                helperText={errors?.password?.message?.toString()}
            />
            <LoadingButton loading={isSubmitting}
                disabled={!isValid}
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
            >
                Sign In
            </LoadingButton>
            <Grid container>
                <Grid item>
                    <Link href="/register">
                        {"Don't have an account? Sign Up"}
                    </Link>
                </Grid>
            </Grid>
        </Box>
    </Container>
    )
}