"use client";

import { useSession, signIn, signOut } from "next-auth/react"

export default function LoginForm() {
    const session = useSession();

    if (session.data?.user) {
        return (
            <>
                Signed in as {session.data.user.email} <br />
                <button onClick={() => signOut()}>Sign out</button>
            </>
        )
    }

    return (
        <>
            <button onClick={() => signIn()}>Sign in</button>
        </>
    )
}