import { useQuery } from "@tanstack/react-query";

const CompleteMeme = () => {
    const result = useQuery(["completeMeme", ])
    return (
        <div>Complete Meme Rendered Here</div>
    );
}