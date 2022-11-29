import { ErrorMessage, Formik } from "formik";
import { observer } from "mobx-react";
import { Form } from "semantic-ui-react";
import { useStore } from "../../stores/store";
import "./game.css";
import * as Yup from 'yup';

export default observer(function GameFieldForm(){
    const {shootStore} = useStore();
    const token = localStorage.getItem('token');

    const onSubmit = async (values, {setErrors}) => {
        shootStore.fire(values)
        .catch(error => setErrors({error: "There is no such cell on the field!"}));
    }

    const schema = Yup.object().shape({
        x: Yup.number()
            .required("Required")
            .min(1, "Min number is 1")
            .max(10, "Max number is 10")
            .typeError("Must be number"),
        y: Yup.number()
            .required("Required")
            .min(1, "Min number is 1")
            .max(10, "Max number is 10")
            .typeError("Must be number"),
    });

    return(
        <>
        <div>
            <Formik
                validationSchema={schema}
                initialValues={{ x: 0, y: 0, token, error: null}}
                onSubmit = {onSubmit}
                >
                {({ values, handleSubmit, handleBlur, handleChange, isSubmitting, errors}) => (
                <Form className="form" onSubmit={() => {handleSubmit()}}>
                    <Form.Group>
                        <div className="error-container">
                            <p className="error">
                                <ErrorMessage name="x"></ErrorMessage>
                            </p>
                        </div>
                        <div>
                            <p className="error">
                                <ErrorMessage name="y"></ErrorMessage>
                            </p>
                        </div>
                    </Form.Group>
                    <Form.Group>
                    <input
                            type="x"
                            name="x"
                            onChange={handleChange}
                            onBlur={handleBlur}
                            placeholder="Enter x"
                            className="form-control"
                        />
                        <input
                            type="y"
                            name="y"
                            onChange={handleChange}
                            onBlur={handleBlur}
                            placeholder="Enter y"
                            className="form-control"
                        />
                    </Form.Group>

                    <Form.Button id="form-btn" loading={isSubmitting} positive content='Shoot' type="submit"></Form.Button>
                </Form>
                )}
            </Formik>
        </div> 
        </>
        )
})