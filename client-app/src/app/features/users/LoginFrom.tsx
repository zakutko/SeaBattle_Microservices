import { observer } from "mobx-react";
import { useNavigate } from "react-router";
import { useStore } from "../../stores/store";
import "../home/homePage.css";
import "./login-register.css";
import { ErrorMessage, Formik } from "formik";
import * as Yup from "yup";
import { Label } from "semantic-ui-react";

export default observer(function LoginForm()
{
    const { userStore } = useStore();
    const navigate = useNavigate();

    const onSubmit = async (values, { setErrors }) =>
    {
        userStore.login(values)
            .catch(setErrors({ error: "User does not exist or invalid password!" }))
            .then(() => navigate("/"))
    }

    const schema = Yup.object().shape({
        email: Yup.string()
            .required("Email is a required field")
            .email("Invalid email format"),
        password: Yup.string()
            .required("Password is a required field")
            .min(8, "Password must be at least 8 characters"),
    });

    return (
        <>
            <Formik
                validationSchema={schema}
                initialValues={{ email: "", password: "", error: "" }}
                onSubmit={onSubmit}
            >
                {({
                    values,
                    handleChange,
                    handleBlur,
                    handleSubmit,
                    errors,
                    isSubmitting
                }) => (
                    <div className="login-register">
                        <div className="form">
                            <form noValidate onSubmit={handleSubmit}>
                                <span>Login</span>

                                <input
                                    type="email"
                                    name="email"
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    value={values.email}
                                    placeholder="Enter email"
                                    className="form-control inp_text"
                                    id="email"
                                />
                                <p className="error">
                                    <ErrorMessage name="email"></ErrorMessage>
                                </p>
                                <input
                                    type="password"
                                    name="password"
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    value={values.password}
                                    placeholder="Enter password"
                                    className="form-control"
                                />
                                <p className="error">
                                    <ErrorMessage name="password"></ErrorMessage>
                                </p>

                                <ErrorMessage
                                    name="error" render={() =>
                                        <Label
                                            style={{ marginBottom: 10 }} basic color='red' content={errors.error}
                                        />}
                                />
                                <button type="submit" disabled={isSubmitting}>Login</button>
                            </form>
                        </div>
                    </div>
                )}

            </Formik>
        </>
    )
})