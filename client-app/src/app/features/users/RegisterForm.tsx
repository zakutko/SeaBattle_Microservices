import { ErrorMessage, Formik } from "formik";
import { observer } from "mobx-react";
import { Label } from "semantic-ui-react";
import { useStore } from "../../stores/store";
import * as Yup from 'yup';
import { useNavigate } from "react-router";
import "./login-register.css";


export default observer(function RegisterForm() {
    const {userStore} = useStore();
    const navigate = useNavigate();

    const onSubmit = async (values, {setErrors}) => {
        userStore.register(values)
        .catch(setErrors({error: "User with such email or username already exists!"}))
        .then(() => userStore.login(values))
        .then(() => navigate("/"))  
    }

    const schema = Yup.object().shape({
        username: Yup.string()
        .required("Username is a required field")
        .min(1, "Username must be at leats 1 characters")
        .max(15, "Username must be at least 15 characters"),
        displayName: Yup.string()
        .required("Displayname is a required field")
        .min(1, "Displayname must be at least 1 characters")
        .max(25, "Displayname must be at least 25 characters"),
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
            initialValues={{ displayName: '', username: '', email: '', password: '', error: "" }}
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
                        <span>Register</span>

                        <input
                            type="displayName"
                            name="displayName"
                            onChange={handleChange}
                            onBlur={handleBlur}
                            value={values.displayName}
                            placeholder="Enter displayname"
                            className="form-control inp_text"
                            id="displayName"
                        />
                        <p className="error">
                            <ErrorMessage name="displayName"></ErrorMessage>
                        </p>

                        <input
                            type="username"
                            name="username"
                            onChange={handleChange}
                            onBlur={handleBlur}
                            value={values.username}
                            placeholder="Enter username"
                            className="form-control inp_text"
                            id="username"
                        />
                        <p className="error">
                            <ErrorMessage name="username"></ErrorMessage>
                        </p>

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
                                style={{marginBottom: 10}} basic color='red' content={errors.error} 
                            />}  
                        />
                        <button type="submit" disabled={isSubmitting}>Register</button>
                    </form>
                </div>
            </div>
        )}

        </Formik>
        </>
    )
})