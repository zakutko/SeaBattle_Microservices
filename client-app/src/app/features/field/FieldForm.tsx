import { ErrorMessage, Formik } from "formik";
import { observer } from "mobx-react";
import { Form } from "semantic-ui-react";
import { FormControl, FormControlLabel, Radio, RadioGroup } from "@mui/material";
import "./field.css";
import * as Yup from 'yup';
import agent from "../../api/agent";

export default observer(function FieldForm(props: any){
    enum SizeOptions {
        "One" = 1,
        "Two" = 2,
        "Three" = 3,
        "Four" = 4
    }

    enum Options {
        "Horizontal" = 1,
        "Vertical" = 2
    }

    const name = 'shipDirection';
    const nameSize = 'shipSize';
    const token = localStorage.getItem('token');

    const onSubmit = async (values: any) => {
        if(token){
            agent.Games.createShipOnField(values)
                .then(response => {
                    alert(response.message);
                })
                .then(() => {
                    props.onClickBuildAShip();
                })
        }
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

    return (
        <>
        <div id="fieldForm">
            <Formik
                validationSchema={schema}
                initialValues={{shipSize: SizeOptions.One, shipDirection: Options.Horizontal, x: 0, y: 0, token, error: ""}}
                onSubmit = {onSubmit}
                >
                {({ values, handleBlur, handleChange ,setFieldValue, handleSubmit, isSubmitting, errors}) => (
                <Form className="form" onSubmit={handleSubmit}>
                    <h2>Ship size:</h2>
                    <FormControl id="shipSizeRadioGroup" component="fieldset">
                        <RadioGroup name={nameSize} value={values.shipSize} onChange={(event) => {
                            setFieldValue(nameSize, event.currentTarget.value)
                        }}>
                            <FormControlLabel value={SizeOptions.One} control={<Radio />} label="One" />
                            <FormControlLabel value={SizeOptions.Two} control={<Radio />} label="Two" />
                            <FormControlLabel value={SizeOptions.Three} control={<Radio />} label="Three" />
                            <FormControlLabel value={SizeOptions.Four} control={<Radio />} label="Four" />
                        </RadioGroup>
                    </FormControl>
                    <h2>Ship direction:</h2>
                    <FormControl id="shipDirectionRadioGroup" component="fieldset">
                        <RadioGroup name={name} value={values.shipDirection} onChange={(event) => {
                            setFieldValue(name, event.currentTarget.value)
                        }}>
                            <FormControlLabel value={Options.Horizontal} control={<Radio />} label="Horizontal" />
                            <FormControlLabel value={Options.Vertical} control={<Radio />} label="Vertical" />
                        </RadioGroup>
                    </FormControl>
                    <h2>Ship Position:</h2>
                    <Form.Group id="shipPositionGroup">
                        <input
                            type="x"
                            name="x"
                            onChange={handleChange}
                            onBlur={handleBlur}
                            value={values.x}
                            placeholder="Enter x"
                            className="form-control"
                        />
                        <input
                            type="y"
                            name="y"
                            onChange={handleChange}
                            onBlur={handleBlur}
                            value={values.y}
                            placeholder="Enter y"
                            className="form-control"
                        />
                    </Form.Group>
                    <Form.Group id="errorMessagesGroup">
                        <div className="error-container">
                            <p className="error">
                                <ErrorMessage name="x"></ErrorMessage>
                            </p>
                        </div>
                        <div className="error-container">
                            <p className="error">
                                <ErrorMessage name="y"></ErrorMessage>
                            </p>
                        </div>
                    </Form.Group>

                    <Form.Button loading={isSubmitting} positive content='Build a ship' type="submit"></Form.Button>
                </Form>
                )}
            </Formik>
        </div> 
        </>
    )
})