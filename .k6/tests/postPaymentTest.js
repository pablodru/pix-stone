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
                cpf: "69168962428"
            },
            account: {
                number: "824634948",
                agency: "1793"
            }
        },
        destiny: {
            key: {
                value: "30990248520",
                type: "Phone"
            }
        },
        amount: generateRandomNumber(1000, 100000)
    }
    const body = JSON.stringify(paymentToCreate);
    const headers = { "Content-Type": "application/json", "Authorization": `Bearer f46ff6b5-6d49-4392-9968-4c9eeba8b6ba` };

    http.post(`http://localhost:8080/payments`, body, { headers });
}

function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}