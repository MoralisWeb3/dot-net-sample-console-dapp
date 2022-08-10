using Moralis;
using Moralis.Web3Api.Models;

namespace ConsoleDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Write("Usage: ConsoleDemo.exe ADDRESS CLIENT_ID");
                return;
            }

            string address = "0xBa878d88c71E0091613798C53B6c72aDd9b9A6a7";
            ChainList chainId = ChainList.eth;

            // Setup Moralis Client
            MoralisClient.ConnectionData = new Moralis.Models.ServerConnectionData()
            {
                ApiKey = "1kXrzei19HNrb3YvkLaBbOAuRo6SGcmGqmlZ2E6FYFZ2QnqO46rn3xsAX6eRMBns"
            };

            Task.Run(async () =>
            {
                await DisplayCryptoData(address, chainId);
            }).Wait();
            
        }

        internal static async Task DisplayCryptoData(string address, ChainList chainId)
        {
            try
            {
                Console.WriteLine($"For address: {address}...\n");

                // Load native balance for address
                NativeBalance bal = await MoralisClient.Web3Api.Account.GetNativeBalance(address, chainId);

                double nativeBal = 0;

                double.TryParse(bal.Balance, out nativeBal);

                Console.WriteLine($"Your native balance is {bal.NativeTokenBalance}");

                // Load ERC20 Token List for address
                List<Erc20TokenBalance> erc20Balnaces = await MoralisClient.Web3Api.Account.GetTokenBalances(address, chainId);

                Console.WriteLine("\n\nYour ERC 20 Tokens:");

                if (erc20Balnaces != null && erc20Balnaces.Count > 0)
                {
                    // Print out each token with symbol and balance.
                    foreach (Erc20TokenBalance tb in erc20Balnaces)
                    {
                        Console.WriteLine($"\t{tb.Symbol} - {tb.Name}: {tb.NativeTokenBalance}"); 
                    }
                }
                else
                {
                    Console.WriteLine("\tNone");
                }

                // Load first 10 NFTs for the address
                NftOwnerCollection nfts = await MoralisClient.Web3Api.Account.GetNFTs(address, (ChainList)chainId, "", null, 10);

                Console.WriteLine("\n\nYour NFTs:");

                if (nfts != null && nfts.Result.Count > 0)
                {
                    // Print out each token with symbol and balance.
                    foreach (NftOwner nft in nfts.Result)
                    {
                        Console.WriteLine($"\t{nft.Name}: {nft.Amount}\n\tMetaData: {nft.Metadata}\n\n");
                    }
                }
                else
                {
                    Console.WriteLine("\tNone");
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }
    }
}


