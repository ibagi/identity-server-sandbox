import NextAuth from "next-auth"
import DuendeIDS6Provider from "next-auth/providers/duende-identity-server6"

const handler = NextAuth({
  providers: [
    DuendeIDS6Provider({
        clientId: process.env.DUENDE_IDS6_ID || 'ssr-client',
        clientSecret: process.env.DUENDE_IDS6_SECRET || 'ssr-client-secret',
        issuer: process.env.DUENDE_IDS6_ISSUER || 'http://localhost:5273'
      })
  ]
})

export { handler as GET, handler as POST }