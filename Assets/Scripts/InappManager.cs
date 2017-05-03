using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class InappManager : MonoBehaviour, IStoreListener
{

    private static IStoreController m_StoreController;          
    private static IExtensionProvider m_StoreExtensionProvider; 

  
    public static string kProductIDConsumable = "consumable";
    public static string kProductIDNonConsumable = "nonconsumable";
    public static string kProductIDSubscription = "subscription";

   

    private void Start()
    {
       
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        
        if (IsInitialized())
        {
           return;
        }

        var module = StandardPurchasingModule.Instance();
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;

        var builder = ConfigurationBuilder.Instance(module);
        builder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApt4pBdcQu9EIkVPKgdM6AgncNM3fDejy3T/bs0A8W592XgP7sqe/+uz8KymHKV9uE6Uxtwu9kmbPE7CIG26x3SDBl3ePmcpYYj01RKkrkYZF9ZRFph9YpXpmrp44hiRdG8smZGP2afiZVCY7QWavV9tAtgLU2P82o+f/SdFDzxNSzBvqiFPDFZe7r8qw0eY0fadOg9G+lpuhPXQ7/6VGpaPLG5sQQV0J0HYMnzNT8EFQ/hXYQOcauggWjIJQs8dTpTghCnaCQ53XOKnazkHwF06v8b856JcOoDQcPzo5DBoawn2mKGgYeKiheQES/mp+adcRaURKuFxrHRM4VJZTRQIDAQAB");
        



        builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
        
        builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
        
        builder.AddProduct(kProductIDSubscription, ProductType.Subscription);

     
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
       
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyConsumable()
    {
       BuyProductID(kProductIDConsumable);
    }


    public void BuyNonConsumable()
    {
        BuyProductID(kProductIDNonConsumable);
    }


    public void BuySubscription()
    {
       
        BuyProductID(kProductIDSubscription);
    }


    void BuyProductID(string productId)
    {
      
        if (IsInitialized())
        {
           
            Product product = m_StoreController.products.WithID(productId);

           
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
              
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
       else
        {
           
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
       
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
       
        if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
           
           
        }
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDNonConsumable, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        
        }
        else if (String.Equals(args.purchasedProduct.definition.id, kProductIDSubscription, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
           
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

    
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
