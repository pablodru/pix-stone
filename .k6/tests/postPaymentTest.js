import http from "k6/http";

export const options = {
    stages: [
        { duration: "30s", target: 10 },
        { duration: "20s", target: 15 },
        { duration: "10s", target: 20 },
    ],
};

export default function (){

    const paymentToCreate = {
        origin:{
            user:{
                cpf: "28593648602"
            },
            account: {
                number: "189983764",
                agency: "4857"
            }
        },
        destiny: {
            key: {
                value: "46000287469",
                type: "Phone"
            }
        },
        amount: generateRandomNumber(1000, 100000)
    }
    const body = JSON.stringify(paymentToCreate);
    const headers = { "Content-Type": "application/json", "Authorization": `Bearer f04dee77-8805-4311-ae5d-367d03cf083b` };

    http.post(`http://localhost:8080/payments`, body, { headers });
}

function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}