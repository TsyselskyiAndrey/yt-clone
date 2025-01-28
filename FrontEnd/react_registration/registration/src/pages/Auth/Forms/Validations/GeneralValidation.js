import is from 'is_js'
import validator from "validator";

export default function validateControl(value, validation, name, password = "") {
    if (!validation) {
        return [true, ""]
    }
    if (validation.required && value.trim() === '') {
        return [false, `* This field is required.`]
    }
    if (validation.allowSpaces == false && /\s/.test(value)) {
        return [false, `* This field cannot contain spaces. Please remove any spaces and try again.`]
    }
    if (validation.email && is.not.email(value)) {
        return [false, `* The email address you entered is invalid.`]
    }
    if(validation.allowNums === false && /[0-9]/.test(value)){
        return [false, `* The ${name} mustn't contain any numbers.`]
    }
    if (validation.allowSymbols === false && /[^a-zA-Z0-9 ]/.test(value)) {
        return [false, `* The ${name} mustn't contain any special symbols or non-latin letters.`]
    }
    if (validation.requireNums && /\d/.test(value) === false) {
        return [false, `* The ${name} must contain numbers.`]
    }
    if (validation.requireBothCases && (/[A-Z]/.test(value) === false || /[a-z]/.test(value) === false)) {
        return [false, `* The ${name} must contain both upper and lower case letters.`]
    }
    if (validation.minLength && value.length < validation.minLength) {
        return [false, `* The ${name} must contain at least ${validation.minLength} symbols.`]
    }
    if (validation.maxLength && value.length > validation.maxLength) {
        return [false, `* The ${name} must contain less than ${validation.maxLength} symbols.`]
    }
    if (validation.code && value.length != 6) {
        return [false, `* The code is not complete.`]
    }
    if (validation.date) {
        const dateCopy = value.replaceAll(" ", "");
        const parts = dateCopy.split('/')

        if(!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateCopy)){
            return [false, `* Enter the complete date.`]
        }

        const day = parts[1]
        const month = parts[0]
        const year = parts[2]
        if(validator.isDate(`${year}/${month}/${day}`) == false){
            return [false, `* The date is not valid.`]
        }

        const dayPart = parseInt(parts[1], 10);
        const monthPart = parseInt(parts[0], 10) - 1;
        const yearPart = parseInt(parts[2], 10);
        const date = new Date(yearPart, monthPart, dayPart);
        if(date.getFullYear() > new Date().getFullYear()){
            return [false, `* Are you from the future?`]
        }
        if(date.getFullYear() + 13 > new Date().getFullYear()){
            return [false, `* You are too young.`]
        }
        if(date.getFullYear() + 140 < new Date().getFullYear()){
            return [false, `* You are too old.`]
        }

    }
    if (validation.repassword && value != password) {
        return [false, `* The passwords do not match.`]
    }
    return [true, ""]
}