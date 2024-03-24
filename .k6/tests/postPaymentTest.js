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
                cpf: "14210557925"
            },
            account: {
                number: "521352960",
                agency: "7251"
            }
        },
        destiny: {
            key: {
                value: "49004118344",
                type: "Phone"
            }
        },
        amount: generateRandomNumber(1000, 100000)
    }
    const body = JSON.stringify(paymentToCreate);
    const headers = { "Content-Type": "application/json", "Authorization": `Bearer 0acfb721-29e4-46a2-99ff-8c910db85de8` };

    http.post(`http://localhost:8080/payments`, body, { headers });
}

function generateRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}